<template>
    <div>
        <br>
      <h2>Detalles del Libro</h2>
      <div v-if="isLoading">Cargando...</div>
      <div v-else-if="error">Error: {{ error }}</div>
      <div v-else>
        <p><strong>Título:</strong> {{ book?.Title }}</p>
        <p><strong>Dirección del Archivo:</strong> {{ book?.FileDirection }}</p>
        <p><strong>Fecha de Publicación:</strong> {{ book?.PublicationDate }}</p>
        <p><strong>ID del Autor del Libro:</strong> {{ book?.BookAuthorGuid }}</p>
        <p><strong>Nombre del Autor:</strong> {{ book?.AuthorName }}</p>
        <p><strong>Apellido del Autor:</strong> {{ book?.AuthorLastName }}</p>
        <p><strong>Fecha de Nacimiento del Autor:</strong> {{ book?.AuthorBirthdate }}</p>
      </div>


      <v-btn @click="$emit('cancel');" color="Success">Cerrar</v-btn>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, PropType, ref } from 'vue';
  import bookService from '~/services/BookServices';
  
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
    props: {
      bookId: {
        type: String as PropType<string>,
        required: true,
      },
    },
    data() {
      return {
        book: null as Book | null,
        isLoading: false,
        error: '',
      };
    },
    mounted() {
      this.fetchBook();
    },
    methods: {
      async fetchBook() {
        this.isLoading = true;
        try {
          this.book = await bookService.getBookById(this.bookId);
        } catch (err) {
          this.error = 'No se pudo cargar el libro.';
          console.error('Error al obtener el libro:', err);
        } finally {
          this.isLoading = false;
        }
      },
    },
  });
  </script>
  