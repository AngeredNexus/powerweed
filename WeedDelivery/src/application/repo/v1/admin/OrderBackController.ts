import {InternalRepo} from "@/application/repo/v1/back-repo";

export class OrderBackController {
    
    repository = new InternalRepo("order/");
    
    GET_PROGRESS_ORDERS(){}
    GET_DONE_ORDERS()    {}
    
    POST_ORDER_UPD() {}
    POST_ORDER_RMV() {}
    POST_ORDER_ADD() {}
}