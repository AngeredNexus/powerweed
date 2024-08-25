namespace WeedDelivery.Backend.Common.Utils;

public static class GuidExtensions
{
     public static Guid Sequential(this Guid guid)
     {
          return RT.Comb.Provider.PostgreSql.Create();
     }
     
     public static Guid Sequential()
     {
          return RT.Comb.Provider.PostgreSql.Create();
     }
}