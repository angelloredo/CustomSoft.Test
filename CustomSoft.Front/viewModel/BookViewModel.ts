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
    FileDirection?: string = '';
    PublicationDate?: Date = new Date();
    BookAuthorGuid?: string = '';
    AuthorName?: string = '';
    AuthorLastName?: string = '';
    AuthorBirthdate?: string = '';
    FileName?: string = '';
  }
