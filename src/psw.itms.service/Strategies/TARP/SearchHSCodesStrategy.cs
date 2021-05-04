
// using System;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;
// using System.Text;
// using System.Net.Http.Headers;

// using AutoMapper;
// using PSW.ITMS.Service.DTO;
// using PSW.ITMS.Service.Command;
// using PSW.ITMS.Data.Entities;
// // using PSW.ITMS.Service.Mapper;


// namespace PSW.ITMS.Service.Strategies
// {
//     public class SearchHSCodesStrategy : ApiStrategy<SearchHSCodesRequestDto, SearchHSCodesResponseDto>
//     {

//         #region Properties 
//         #endregion 

//         #region Constructors 
//         public SearchHSCodesStrategy(CommandRequest request) : base(request)
//         {
//             this.Reply = new CommandReply();
//         }
//         #endregion 

//         #region Distructors 
//         ~SearchHSCodesStrategy()
//         {

//         }
//         #endregion 

//         #region Strategy Excecution  

//         public override CommandReply Execute()
//         {
//             try 
//             {
//                 ResponseDTO = new SearchHSCodesResponseDto();
//                 ResponseDTO.HSCodes = new List<HSCodesData>();

//                 // Query Database  
//                 List<HSCodeTARP> HSCodeList = SearchHSCodes(RequestDTO);
//                 ResponseDTO.HSCodes = HSCodeList.Select(item => this.Command._mapper.Map<HSCodesData>(item)).ToList();

//                 // Send Command Reply 
//                 return OKReply();
//             }
//             catch (Exception ex)
//             {
//                 return InternalServerErrorReply(ex);
//             }
//         }

//         #endregion


//         #region Methods  

//         public List<HSCodeTARP> SearchHSCodes(SearchHSCodesRequestDto req)
//         {
//             try
//             {
//                 // Begin Transaction  
//                 this.Command.UnitOfWork.BeginTransaction();

//                 // Query Database 
//                 List<HSCodeTARP> HSCodeList =this.Command.UnitOfWork.HSCodeTARPRepository.SearchHSCodes(new {
//                     HSCode = req.HSCode ?? "",
//                     HSCodeExt = req.PCTCode ?? "",
//                     ItemDescription = req.CommodityName,
//                     TechnicalName = req.TechnicalName
//                 }, req.AgencyId);

//                 // Commit Transaction  
//                 this.Command.UnitOfWork.Commit();

//                 return HSCodeList;
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }
//         }

//         #endregion 

//     }
// }
