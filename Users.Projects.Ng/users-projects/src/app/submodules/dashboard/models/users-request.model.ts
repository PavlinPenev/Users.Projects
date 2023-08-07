export interface UsersRequest {
        searchTerm: string,
        orderBy: string,
        isDescending: boolean,
        dateAddedFrom: string | null,
        dateAddedTo: string | null,
        skip: number,
        take: number
}