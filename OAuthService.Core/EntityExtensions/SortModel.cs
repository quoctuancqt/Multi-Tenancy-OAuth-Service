using OAuthService.Core.Enums;

namespace OAuthService.Core.EntityExtensions
{
    public class SortModel
    {
        public SortModel(string sortField, OrderByTypeEnum sortDirection)
        {
            SortField = sortField;
            OrderByType = sortDirection;
        }

        public string SortField { get; set; }

        public OrderByTypeEnum OrderByType { get; set; }
    }
}
