import axios, {AxiosInstance} from "axios";
import {trim} from "@/application/utils/stringExt";

export interface RequestParam {
    name: string,
    value: any
}

export class BackendRepository {
    
    private apiClient: AxiosInstance;
    private clientReady: boolean = false;
    
    private readonly _baseUrl: string;
    
    public BaseUrl() {
        return this._baseUrl;
    }
    
    public POST(url: string, data: RequestParam[])
    {
        if(!this.clientReady)
            throw `CLIENT NOT READY FOR POST (url: ${url}); DATA: ${data}`;
        
        let requestData = this.makeFormData(data);
        return this.apiClient.post(url, requestData);
        
    }
    public GET(url: string, params: RequestParam[])
    {
        if(!this.clientReady)
            throw `CLIENT NOT READY FOR GET (url: ${url}); Params: ${params}`;
        
        let paramUrl = this.makeQueryUrl(params);
        let fullUrl = `${url}${paramUrl}`
        
        return this.apiClient.get(fullUrl).then();
    }
    
    public constructor(baseUrl: string) {
        this._baseUrl = baseUrl;
        this.apiClient = axios.create({
            baseURL: this._baseUrl,
            headers: {
                "Content-type": "application/json",
            },
        });
    }


    private makeFormData(data: RequestParam[]): FormData {
        
        let rqfd = new FormData();
        
        data.forEach(function (value) {
           rqfd.append(value.name, value.value) 
        });
        
        return rqfd;
    }
    
    private makeQueryUrl(params: RequestParam[]): string
    {
        var paramsUrl = "?";
        
        params.forEach(function (value) {
            paramsUrl += `${value.name}=${value.value}&`;            
        })

        return trim(paramsUrl, "&");
    }
}