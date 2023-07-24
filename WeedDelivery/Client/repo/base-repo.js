import axios from 'axios';

export default function client(base_url) {
    return {
        
        host_url: base_url,
        
        async get(url, params) {
            
            let full_url = `${this.host_url}/${url}`;
            let result = await axios.get(full_url, params).then(function (response) {
                return response;
            });
            
            return result;
        },

        async post(url, data) {

            let full_url = `${this.host_url}/${url}`;

            let result = await axios.post(full_url, data).then(function (response) {
                return response;
            });
            
            return result;
        }
    }
}