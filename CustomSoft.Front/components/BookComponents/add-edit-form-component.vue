<template>
    <v-form ref="form" v-model="valid" lazy-validation>
      <v-text-field v-model="book.Title" label="Título" required></v-text-field>
      <v-text-field v-model="book.FileDirection" label="Dirección del Archivo"></v-text-field>
      <v-text-field v-model="book.PublicationDate" label="Fecha de Publicación"></v-text-field>
      <v-text-field v-model="book.BookAuthorGuid" label="ID del Autor del Libro"></v-text-field>
      <v-text-field v-model="book.AuthorName" label="Nombre del Autor"></v-text-field>
      <v-text-field v-model="book.AuthorLastName" label="Apellido del Autor"></v-text-field>
      <v-text-field v-model="book.AuthorBirthdate" label="Fecha de Nacimiento del Autor"></v-text-field>
  
      <v-btn @click="saveBook" :disabled="!valid" color="primary">{{ buttonText }}</v-btn>
      <v-btn @click="resetForm" color="secondary">Resetear</v-btn>
      <v-btn @click="resetForm;$emit('cancel');" color="warning">Cancelar</v-btn>
    </v-form>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref } from 'vue';
  import bookService from '~/services/BookServices';
  
  interface Book {
    // Define las propiedades de acuerdo a tu modelo de datos
    BookId?: string; // Opcional para edición
    Title: string;
    FileDirection?: string;
    PublicationDate?: string;
    BookAuthorGuid?: string;
    AuthorName?: string;
    AuthorLastName?: string;
    AuthorBirthdate?: string;
  }
  
  export default defineComponent({
    props: {
      initialBook: Object as () => Book, // Prop para prellenar el formulario en caso de edición
    },
    data() {
      return {
        book: this.initialBook || {} as Book,
        valid: false,
      };
    },
    computed: {
      buttonText(): string {
        return this.initialBook ? 'Actualizar' : 'Agregar'; // Cambia el texto del botón según si estamos editando o agregando un libro
      },
    },
    methods: {
      async saveBook() {
        try {
          if (this.initialBook) {
            // Si estamos editando un libro, llamamos al método de actualización
            await bookService.updateBook(this.book.BookId as string, this.book as any);
            console.log('Libro actualizado con éxito');
          } else {
            // Si estamos agregando un nuevo libro, llamamos al método de inserción
            await bookService.insertBook(this.book);
            console.log('Libro agregado con éxito');
          }
          // Emitimos un evento para indicar que se ha agregado o actualizado un libro
          this.$emit('bookSaved');
        } catch (error) {
          console.error('Error al guardar el libro:', error);
          // Puedes manejar el error aquí, por ejemplo, mostrando un mensaje al usuario
        }
      },
      resetForm() {
        // Reseteamos el formulario
        this.$refs.form?.reset();
        this.book = {} as Book;
        this.valid = false;
      },
    },
  });
  </script>
  