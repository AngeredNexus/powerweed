import {InternalRepo} from "@/application/repo/v1/back-repo";

export class CustomerBackController {
    
    repository = new InternalRepo("customer/");
    
    // UNUSED NOW!!!s
    GET_CATEGORIES(){}
    
    async POST_SEARCH(url: string, params: any){
        
        return await this.repository.baseRepo.GET(url, params)
            .then(
                (resp) => resp);
        
    }
    POST_ORDER(){}
    
}