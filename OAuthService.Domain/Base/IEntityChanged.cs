namespace OAuthService.Domain.Base
{
    using System.Collections.Generic;

    public interface IEntityChanged
    {
        IEnumerable<FieldChanged> FieldsChanged { get; set; }
    }
}
