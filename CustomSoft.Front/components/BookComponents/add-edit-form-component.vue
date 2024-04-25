<template>
    <v-form ref="form2" v-model="valid" lazy-validation>
        <v-text-field v-model="book.Title" label="Título" required></v-text-field>
        <v-text-field v-model="book.PublicationDate" label="Fecha de Publicación" type="date"></v-text-field>
        <v-text-field v-model="book.BookAuthorGuid" label="ID del Autor del Libro"></v-text-field>

        <!-- Componente v-file-input -->
        <v-file-input v-if="selectedId" v-model="file" label="Seleccionar archivo" accept="*"></v-file-input>

        <v-btn @click="saveBook" :disabled="!valid" color="primary">{{ buttonText }}</v-btn>
        <v-btn @click="resetForm" color="secondary">Resetear</v-btn>
        <v-btn @click="resetForm; $emit('cancel');" color="warning">Cancelar</v-btn>
    </v-form>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import bookService from '~/services/BookServices';
import { Book } from '~/viewModel/BookViewModel';
import { BookViewModel } from '../../viewModel/BookViewModel';

export default defineComponent({
    props: {
        selectedId: String,
    },
    data() {
        return {
            book: new BookViewModel(),
            valid: false,
            file: null,
        };
    },
    computed: {
        buttonText(): string {
            return this.selectedId ? 'Actualizar' : 'Agregar';
        },
    },
    async mounted() {
        if (this.selectedId) {
            try {
                this.book = await bookService.getBookById(this.selectedId);
            } catch (error) {
                console.error('Error al cargar el libro:', error);
            }
        } else {
            this.book = new BookViewModel();
        }
    },
    methods: {
        async saveBook() {
            try {
                if (this.selectedId) {
                    await bookService.updateBook(this.selectedId, (this.book as any));

                    // Agregar el archivo si se proporciona
                    if (this.file) {
                        await bookService.uploadFile(this.book.BookId, this.file as File);
                        console.log('Archivo seleccionado:', this.file);
                    }
                    console.log('Libro actualizado con éxito');
                } else {

                    await bookService.insertBook(this.book);
                    console.log('Libro agregado con éxito');
                }
                this.$emit('bookSaved');
            } catch (error) {
                console.error('Error al guardar el libro:', error);
            }
        },
        resetForm() {
            (this.$refs.form2 as any).reset();
            this.book = new BookViewModel();
            this.valid = false;
        },
    },
});
</script>