# Final Project

- Josh Birch
- Mac Mungham

## Product Service

### Build with Docker:

```sh
docker build -t product-service ./product-service
```

### GET /products

```json
[{ "id": 1, "name": "Widget", "description": "A widget", "price": 9.99 }]
```

### GET /products/:id

```json
{ "id": 1, "name": "Widget", "description": "A widget", "price": 9.99 }
```

Returns `404` if not found.

## Order Service

### Build with Docker:

```sh
docker build -t product-service ./product-service
```

### GET /orders

```json
[{ "id": 1, "productIds": [1, 2], "orderDate": "2024-01-01T00:00:00Z", "totalPrice": 19.98 }]
```

### GET /orders/:id

```json
{ "id": 1, "productIds": [1, 2], "orderDate": "2024-01-01T00:00:00Z", "totalPrice": 19.98 }
```

Returns `404` if not found.

### POST /orders

Request:

```json
{ "productIds": [1, 2], "orderDate": "2024-01-01T00:00:00Z" }
```

Response `201`:

```json
{ "id": 1, "productIds": [1, 2], "orderDate": "2024-01-01T00:00:00Z", "totalPrice": 19.98 }
```

Returns `400` if any product ID is invalid.

Calls `GET /products/:id` on **product-service** for each product ID to resolve prices. `totalPrice` is the sum of all resolved product prices.
