
using Application.DTOs;
using BoxFactoryApp;

namespace Application.Interfaces;

public interface IBoxRepository
{
     List<Box> GetAllBoxes();
     Box CreateNewBox(Box box);
     Box GetBoxById(int id);
     void RebuildDB();
     Box UpdateBox(Box box);
     Box DeleteBox(int id);
}