import axios from 'axios';

export default function client(base_url) {
    return {
        client: axios.create({
           baseURL: base_url,
           withCredentials: true,
            headers: {
                "Content-Type": 'application/json',
                "Accept": "/",
                "Cache-Control": "no-cache"
            }
        }),
        
        async get(url, params) {
            
            let result = await this.client.get(url, { params: params }).then(function (response) {
                return response;
            });
            
            return result;
        },

        async post(url, data, params) {

            let result = await this.client.post(url, data, { params: params }).then(function (response) {
                return response;
            });
            
            return result;
        }
    }
}