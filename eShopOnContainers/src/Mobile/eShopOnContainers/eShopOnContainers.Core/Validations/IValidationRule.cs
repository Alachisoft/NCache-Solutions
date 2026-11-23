// Copyright (c) 2017 Microsoft Corp.
namespace eShopOnContainers.Core.Validations
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }

        bool Check(T value);
    }
}
