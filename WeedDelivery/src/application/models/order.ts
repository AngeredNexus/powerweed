import OrderItem from "@/application/models/order-item";

export default class Order {
    
    firstname: string
    lastname: string
    phoneNumber: string
    address: string
    deliveryMan: string
    Items: OrderItem[]
    
}