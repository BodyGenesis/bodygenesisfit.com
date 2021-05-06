namespace BodyGenesis.Core.Entities.Queries
{
    public class CustomerByAuth0UserIdQuery : QueryBase<Customer>
    {
        public CustomerByAuth0UserIdQuery(string auth0UserId)
        {
            FilterExpression = (customer) => customer.Auth0UserId == auth0UserId;
        }
    }
}
