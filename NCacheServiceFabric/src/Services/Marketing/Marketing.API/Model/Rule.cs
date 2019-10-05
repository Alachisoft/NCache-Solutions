using System;

namespace Microsoft.eShopOnContainers.Services.Marketing.API.Model
{
    [Serializable]
    public abstract class Rule
    {
        public int Id { get; set; }

        public int CampaignId { get; set; }

        public Campaign Campaign { get; set; }
        
        public string Description { get; set; }
    }

    [Serializable]
    public class UserProfileRule : Rule
    {
    }

    [Serializable]
    public class PurchaseHistoryRule : Rule
    {
    }

    [Serializable]
    public class UserLocationRule : Rule
    {
        public int LocationId { get; set; }
    }
}