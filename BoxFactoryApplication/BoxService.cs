using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using BoxFactoryApp;
using FluentValidation;

namespace Application;

public class BoxServive : IBoxService
{
    private readonly IBoxRepository _boxRepository;
    private readonly IValidator<BoxDTOs> _postValidator;
    private readonly IValidator<Box> _boxValidator;
    private readonly IMapper _mapper;
    
    public BoxServive(
        IBoxRepository repository,
        IValidator<BoxDTOs> postValidator,
        IValidator<Box> productValidator,
        IMapper mapper)
    {
        _mapper = mapper;
        _postValidator = postValidator;
        _boxValidator = productValidator;
        _boxRepository = repository;
    }
    
    
    public List<Box> GetAllNBoxes()
    {
        return _boxRepository.GetAllBoxes();
    }

    public Box CreateNewBox(BoxDTOs dto)
    {
        var validation = _postValidator.Validate(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());

        return _boxRepository.CreateNewBox(_mapper.Map<Box>(dto));

    }

    public Box GetBoxById(int id)
    {
        return _boxRepository.GetBoxById(id);
    }

    public void RebuildDB()
    {
        _boxRepository.RebuildDB();
    }

    public Box UpdateBox(int id, Box box)
    {
        if (id != box.Id)
            throw new ValidationException("ID in body and route are different");
        var validation = _boxValidator.Validate(box);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        return _boxRepository.UpdateBox(box);

    }

    public Box DeleteBox(int id)
    {
        return _boxRepository.DeleteBox(id);
    }
}