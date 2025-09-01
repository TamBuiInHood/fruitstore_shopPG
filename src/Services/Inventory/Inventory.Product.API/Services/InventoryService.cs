using AutoMapper;
using Inventory.Product.API.Entities;
using Inventory.Product.API.Extensions;
using Inventory.Product.API.Repository.Abstraction;
using Inventory.Product.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.DTOs.InventoryDTO;
using Shared.SeedWork;
using System.Net.WebSockets;

namespace Inventory.Product.API.Services
{
    public class InventoryService : MongoRepository<InventoryEntry>, IInventoryServices
    {
        private readonly IMapper _mapper;
        public InventoryService(IMongoClient client, MongoDbSettings settings, IMapper mapper) : base(client, settings)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await FindAll()
                .Find(x => x.ItemNo.Equals(itemNo, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
            var result = _mapper.Map<IEnumerable<InventoryEntryDto>>(entities);
            
            return result;
        }

        public async Task<PageList<InventoryEntryDto>> GetAllbyItemNoPagin(GetInventoryPagingQuery query)
        {
            var filterSearchTerms = Builders<InventoryEntry>.Filter.Empty;
            var filterItemNo = Builders<InventoryEntry>.Filter.Eq(x => x.ItemNo, query.ItemNo());
            if (!string.IsNullOrEmpty(query.SearchTerm))
                filterSearchTerms = Builders<InventoryEntry>.Filter.Eq(x => x.DocumentNo, query.SearchTerm);

            var andFilter = filterItemNo & filterSearchTerms;
            var pageList = await  PaginatedListAsync(filter: andFilter, query.PageIndex,query.PageSize);
            var items = _mapper.Map<IEnumerable<InventoryEntryDto>>(pageList);

            var result = new PageList<InventoryEntryDto>(items,
                                           pageList.GetMetaData().TotalItems,
                                           query.PageIndex,
                                           query.PageSize);

            return result; 
        }


        public async Task<InventoryEntryDto> GetByIdAsync(string id)
        {
            FilterDefinition<InventoryEntry> filter = Builders<InventoryEntry>.Filter.Eq(x => x.Id, id);
            var entity = await FindAll().Find(filter).FirstOrDefaultAsync();
            var reuslt = _mapper.Map<InventoryEntryDto>(entity);
            return reuslt;
        }

        public async Task<double> GetAvailableQuantity(string itemNo)
        {
            var entities = await FindAll()
                .Find(x => x.ItemNo.Equals(itemNo, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
            var result = entities.Sum(x => x.Quantity);

            return result;
        }
        public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
        {
            var itemToAdd = new InventoryEntry(ObjectId.GenerateNewId().ToString())
            {
                ItemNo = itemNo,
                Quantity = model.Quantity,
                DocumentType = model.DocumentType,

            };
            await CreateAsync(itemToAdd);
            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            
            return result;
        }
    }
}
