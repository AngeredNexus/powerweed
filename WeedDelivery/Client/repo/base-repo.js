import axios from 'axios';

export default function client(base_url) {
    return {
        client: axios.create({
           baseURL: base_url,
           withCredentials: true,
            headers: {
                "Content-Type": 'application/json',
                "Accept": "/",
                "Cache-Control": "no-cache",
               Cookie: document.cookie
            }
        }),
        
        async get(url, params, cookies) {
            
            let result = await this.client.get(url, { params: params }).then(function (response) {
                return response;
            });
            
            return result;
        },

        async post(url, data, cookies) {

            let result = await this.client.post(url, data).then(function (response) {
                return response;
            });
            
            return result;
        }
    }
}