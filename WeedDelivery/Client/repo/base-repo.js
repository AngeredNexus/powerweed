import axios from 'axios';

export default function client(base_url) {
    return {
        
        host_url: base_url,
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
            
            let full_url = `${url}`;
            let result = await this.client.get(full_url, { params: params }).then(function (response) {
                return response;
            });
            
            return result;
        },

        async post(url, data, cookies) {

            let full_url = `${url}`;

            let result = await this.client.post(full_url, data).then(function (response) {
                return response;
            });
            
            return result;
        }
    }
}