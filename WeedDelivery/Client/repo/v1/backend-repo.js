import client from '../base-repo'

const backend_url= `${process.env.APP_API_HOST}/api/v1`;

export default function back_repo(controller_url) {
    return {
        
        c_url: controller_url,
        repo_client: client(backend_url),
        
        async post(url, data){

            let full_url = `${this.c_url}/${url}`;
            let response = await this.repo_client.post(full_url, data);
            return response;
        },
        async get(url, params){

            let full_url = `${this.c_url}/${url}`;
            let response = await this.repo_client.get(full_url, params);
            return response;
        },
    }
}