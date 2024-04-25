<template>
    <div>
     
      <h2>Listado de Libros</h2>
      <!-- Componente de Agregar Libro -->
      <v-dialog v-model="showAddForm" max-width="500" v-on:close="showAddForm = false">
        <template v-slot:activator="{ on }">
          <v-btn color="primary" dark v-on="on">Agregar Libro</v-btn>
        </template>
        <v-card>
          <v-card-title>Agregar Nuevo Libro</v-card-title>
          <v-card-text>
            <AddBookForm v-if="showAddForm" @bookSaved="onBookAdded" @cancel="showAddForm = false" />
          </v-card-text>
        </v-card>
      </v-dialog>


      <v-dialog v-model="showDetailsDialog" max-width="500" v-on:close="showDetailsDialog = false">
     
        <v-card>
          <v-card-text>
            <BookDetailsDialog :bookId="selectedBookId" v-if="showDetailsDialog" @close="closeDetailsDialog()" />
          </v-card-text>
        </v-card>
      </v-dialog>
  
      <!-- Componente de Detalles -->
    
  
      <v-data-table
        :headers="headers"
        :items="books"
        item-key="BookId"
      >
        <template v-slot:item="{ item }">
          <tr>
            <td>{{ item.Title }}</td>
            <td>{{ item.FileDirection }}</td>
            <td>{{ item.PublicationDate }}</td>
            <td>{{ item.BookAuthorGuid }}</td>
            <td>{{ item.AuthorName }}</td>
            <td>{{ item.AuthorLastName }}</td>
            <td>{{ item.AuthorBirthdate }}</td>
            <td>
              <v-icon @click="openDetailsDialog(item.BookId)">mdi-eye</v-icon>
              <v-icon @click="deleteBook(item.BookId)">mdi-delete</v-icon>
            </td>
          </tr>
        </template>
      </v-data-table>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref } from 'vue';
  import bookService from '~/services/BookServices';
  import AddBookForm from '~/components/BookComponents/add-edit-form-component.vue';
  import BookDetailsDialog from '~/components/BookComponents/details-component.vue'; // Importar el componente de detalles
  
  interface Book {
    // Define las propiedades de acuerdo a tu modelo de datos
    BookId: string;
    Title: string;
    FileDirection?: string;
    PublicationDate?: string;
    BookAuthorGuid?: string;
    AuthorName?: string;
    AuthorLastName?: string;
    AuthorBirthdate?: string;
  }
  
  export default defineComponent({
    components: {
      AddBookForm,
      BookDetailsDialog, // Registrar el componente de detalles
    },
    data() {
      return {
        books: [] as Book[],
        headers: [
          { text: 'Título', value: 'Title' },
          { text: 'Dirección del Archivo', value: 'FileDirection' },
          { text: 'Fecha de Publicación', value: 'PublicationDate' },
          { text: 'ID del Autor del Libro', value: 'BookAuthorGuid' },
          { text: 'Nombre del Autor', value: 'AuthorName' },
          { text: 'Apellido del Autor', value: 'AuthorLastName' },
          { text: 'Fecha de Nacimiento del Autor', value: 'AuthorBirthdate' },
          { text: 'Acciones', value: 'actions' },
        ],
        showAddForm: false,
        showDetailsDialog: false,
        selectedBookId: '',
      };
    },
    methods: {
      async loadBooks() {
        try {
          this.books = await bookService.getBookList();
        } catch (error) {
          console.error('Error al obtener el listado de libros:', error);
          // Puedes manejar el error aquí, por ejemplo, mostrando un mensaje al usuario
        }
      },
      async deleteBook(bookId: string) {
        try {
          await bookService.deleteBook(bookId);
          await this.loadBooks();
          console.log('Libro eliminado con éxito');
        } catch (error) {
          console.error('Error al eliminar el libro:', error);
          // Puedes manejar el error aquí, por ejemplo, mostrando un mensaje al usuario
        }
      },
      openDetailsDialog(bookId: string) {
        this.selectedBookId = bookId;
        this.showDetailsDialog = true;
      },
      closeDetailsDialog() {
        this.showDetailsDialog = false;
      },
      onBookAdded() {
        this.showAddForm = false;
        this.loadBooks();
      },
    },
    async mounted() {
      await this.loadBooks();
    },
  });
  </script>
  