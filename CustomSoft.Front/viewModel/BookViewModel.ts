export interface Book {
    // Define las propiedades de acuerdo a tu modelo de datos
    BookId: string;
    Title: string;
    FileDirection?: string;
    PublicationDate?: Date;
    BookAuthorGuid?: string;
    AuthorName?: string;
    AuthorLastName?: string;
    AuthorBirthdate?: string;
    FileName?: string;
}


export class BookViewModel implements Book {
    BookId: string = '';

    Title: string = '';
    PublicationDate?: Date;
    BookAuthorGuid?: string = '';
    AuthorName?: string = '';
    AuthorLastName?: string = '';
    FileName?: string = '';

    constructor(
        BookId: string,
        Title: string,
        PublicationDate?: Date,
        BookAuthorGuid?: string,
        AuthorName?: string,
        AuthorLastName?: string,
        FileName?: string
    ) {
        this.BookId = BookId;
        this.Title = Title;
        this.PublicationDate = PublicationDate;
        this.BookAuthorGuid = BookAuthorGuid;
        this.AuthorName = AuthorName;
        this.AuthorLastName = AuthorLastName;
        this.FileName = FileName;
    }

    static createEmpty(): BookViewModel {
        return new BookViewModel('','');
    }


}
