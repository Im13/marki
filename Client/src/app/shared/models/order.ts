export interface OrderToCreate {
  basketId: string;
  deliveryMethodId: number;
  shipToAddress: Address;
}

export interface Root {
  id: number
  buyerEmail: string
  orderDate: string
  shipToAddress: Address
  deliveryMethod: string
  shippingPrice: number
  orderItems: OrderItem[]
  subtotal: number
  total: number
  status: string
}

export interface Address {
  fullname: string
  cityOrProvinceId: number
  districtId: number
  wardId: number
  street: string
}

export interface OrderItem {
  productId: number
  productName: string
  pictureUrl: string
  price: number
  quantity: number
}
