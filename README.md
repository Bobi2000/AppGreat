# AppGreat - REST API

## Authorization Endpoint
## `POST /api/users/login`

`Content-Type: application/json`

Returns jwt token which is needed for authentication.

## `POST /api/users/register`

`Content-Type: application/json`

`Body example: {"username": "username", "password": "password"}`

Creates a new user.

## Products Endpoint

### `GET api/products`

Returns all products.

### `GET api/products/1`

Returns a specific product by inputing the product id.

### `POST api/products`

`Content-Type: application/json`

 `Body example: {"name": "Skinny Trousers With Belt", "price": "19,99"}`

 `Authorization: Bearer {token}`

Creates a new product.

### `DELETE api/products/1`

`Authorization: Bearer {token}`

Deletes a specific product.

## Orders Endpoint

### `POST api/orders`

`Content-Type: application/json`

`Body example: {"userId": 3, "productId": 4}`

`Authorization: Bearer {token}`

If this is the first product that the User adds to his Order it will create a new order with the product in it. 

If the user already has an order it will add the product the the current order.

### `POST api/orders/1`

`Content-Type: application/json`

`Body example: {"Status": 2}`

`Authorization: Bearer {token}`

It changes the status of the specified order.

### `GET api/orders/3`

`Authorization: Bearer {token}`

Returns the users order with the price calculated based on the user currency.
