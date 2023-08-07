import { User } from "./user.model";

export interface UsersResponse {
    items: User[],
    totalItemsCount: number
}