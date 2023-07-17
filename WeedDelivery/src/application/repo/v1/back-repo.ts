import {BackendRepository} from "@/application/repo/base-repo";

const url = "http://localhost:55525/api/v1/"

export class InternalRepo {
    baseRepo: BackendRepository;

    constructor(subUrl: string) {
        this.baseRepo = new BackendRepository(`${url}${subUrl}`)
    }


}