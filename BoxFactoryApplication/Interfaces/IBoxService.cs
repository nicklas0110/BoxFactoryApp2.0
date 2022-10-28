using Application.DTOs;
using BoxFactoryApp;

namespace Application.Interfaces;

public  interface IBoxService
{
     List<Box> GetAllNBoxes();
     Box CreateNewBox(BoxDTOs dto);
     Box GetBoxById(int id);
     void RebuildDB();
     Box UpdateBox(int id, Box product);
     Box DeleteBox(int id);
}