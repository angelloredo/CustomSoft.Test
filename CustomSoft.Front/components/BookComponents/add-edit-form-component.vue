<template>
    <v-form ref="form2" v-model="valid">
        <v-text-field v-model="book.Title" label="Título" required
            :rules="[v => !!v || 'El título es requerido']"></v-text-field>
        <v-text-field v-model="book.PublicationDate" label="Fecha de Publicación" type="date" required
            :rules="[v => !!v || 'La fecha de publicación es requerida']">

        </v-text-field>
        <v-text-field v-model="book.BookAuthorGuid" label="ID del Autor del Libro" required
            :rules="[v => !!v || 'El ID del autor es requerido']"></v-text-field>

        <!-- Componente v-file-input -->
        <v-file-input v-if="selectedId" v-model="file" label="Seleccionar archivo"
            accept="application/pdf"></v-file-input>

        <v-btn @click="saveBook" :disabled="!valid" color="primary">{{ buttonText }}</v-btn>
        <v-btn @click="resetForm" color="secondary">Resetear</v-btn>
        <v-btn @click="resetForm; $emit('cancel');" color="warning">Cancelar</v-btn>
    </v-form>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { BookViewModel } from '../../viewModel/BookViewModel';
import BookService from '~/services/BookServices';

export default defineComponent({
    props: {
        selectedId: String,
    },
    data() {
        return {
            book: new BookViewModel(),
            valid: false,
            enabledAddButton: false,
            file: null,
            bookService: new BookService(null)
        };
    },
    computed: {
        buttonText(): string {
            return this.selectedId ? 'Actualizar' : 'Agregar';
        },
    },
    async mounted() {
        this.bookService = new BookService(this);
        if (this.selectedId) {
            try {
                this.book = await this.bookService.getBookById(this.selectedId);
            } catch (error) {
                console.error('Error al cargar el libro:', error);
            }
        } else {
            this.book = new BookViewModel();
        }
        console.log('Formulario válido:', this.valid);
        this.valid = false;
        this.$watch('valid', () => {
            // Log para verificar si 'valid' se actualiza correctamente
            console.log('Formulario válido:', this.valid);
        });
    },
    methods: {

        async saveBook() {
            try {
                if (this.selectedId) {
                    await this.bookService.updateBook(this.selectedId, (this.book as any));

                    // Agregar el archivo si se proporciona
                    if (this.file) {
                        await this.bookService.uploadFile(this.book.BookId, this.file as any);
                        console.log('Archivo seleccionado:', this.file);
                    }
                    console.log('Libro actualizado con éxito');
                } else {

                    await this.bookService.insertBook(this.book);
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
            this.enabledAddButton = false;
        },
    },
});
</script>