using AutoMapper;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Models.Api.Table;

public class TableDataMappingProfile : Profile
{
    public TableDataMappingProfile()
    {
        // CellData <-> CellDataApi
        CreateMap<CellData, CellDataApi>().ReverseMap();
        CreateMap<ICellData, CellDataApi>()
            .ConvertUsing((src, dest, context) => context.Mapper.Map<CellDataApi>((CellData)src));
        CreateMap<CellDataApi, ICellData>()
            .ConvertUsing((src, dest, context) => context.Mapper.Map<CellData>(src));

        // ColumnData <-> ColumnDataApi
        CreateMap<ColumnData, ColumnDataApi>().ReverseMap();
        CreateMap<IColumnData, ColumnDataApi>()
            .ConvertUsing((src, dest, context) => context.Mapper.Map<ColumnDataApi>((ColumnData)src));
        CreateMap<ColumnDataApi, IColumnData>()
            .ConvertUsing((src, dest, context) => context.Mapper.Map<ColumnData>(src));

        // RowData <-> RowDataApi
        CreateMap<RowData, RowDataApi>()
            .ForMember(dest => dest.Cells, opt => opt.MapFrom((src, dest, _, context) =>
                src.Cells.ToDictionary(
                    kvp => kvp.Key,
                    kvp => context.Mapper.Map<CellDataApi>(kvp.Value)
                )));

        CreateMap<RowDataApi, RowData>()
            .ForMember(dest => dest.Cells, opt => opt.MapFrom((src, dest, _, context) =>
                src.Cells.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (ICellData)context.Mapper.Map<CellData>(kvp.Value)
                )));

        CreateMap<IRowData, RowDataApi>()
            .ConvertUsing((src, dest, context) => context.Mapper.Map<RowDataApi>((RowData)src));
        CreateMap<RowDataApi, IRowData>()
            .ConvertUsing((src, dest, context) => context.Mapper.Map<RowData>(src));

        // TableData <-> TableDataApi
        CreateMap<TableData, TableDataApi>()
            .ForMember(dest => dest.Columns, opt => opt.MapFrom(src => src.Columns))
            .ForMember(dest => dest.Rows, opt => opt.MapFrom(src => src.Rows))
            .ForMember(dest => dest.TableGroups, opt => opt.MapFrom(src => src.TableGroups));

        CreateMap<TableDataApi, TableData>()
            .ForMember(dest => dest.Columns, opt => opt.MapFrom(src => src.Columns))
            .ForMember(dest => dest.Rows, opt => opt.MapFrom(src => src.Rows))
            .ForMember(dest => dest.TableGroups, opt => opt.MapFrom(src => src.TableGroups));

        CreateMap<ITableData, TableDataApi>()
            .ConvertUsing((src, dest, context) => context.Mapper.Map<TableDataApi>((TableData)src));
        CreateMap<TableDataApi, ITableData>()
            .ConvertUsing((src, dest, context) => context.Mapper.Map<TableData>(src));
    }
}
