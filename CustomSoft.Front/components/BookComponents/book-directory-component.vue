<template>
    <div>
      <h2>Directorio de libros</h2>
      <br>
      <v-treeview :items="treeItems" activatable selectable :open="openNodes">
        <template v-slot:prepend="{ item }">
          <v-icon>{{ item.type === 'directory' ? 'mdi-folder' : 'mdi-file' }}</v-icon>
        </template>
        <template v-slot:label="{ item }">
          <span @click="toggleNode(item)">{{ item.name }}</span>
          <v-icon @click="deleteItem(item)" class="ml-2">mdi-delete</v-icon>
        </template>
      </v-treeview>
      <v-dialog v-model="dialog" max-width="600">
        <v-card>
          <v-card-title>Agregar Elemento</v-card-title>
          <v-card-text>
            <v-text-field v-model="newItemName" label="Nombre del Elemento"></v-text-field>
            <v-radio-group v-model="newItemType">
              <v-radio value="directory" label="Directorio"></v-radio>
              <v-radio value="file" label="Archivo"></v-radio>
            </v-radio-group>
          </v-card-text>
          <v-card-actions>
            <v-btn @click="addItem" color="primary">Agregar</v-btn>
            <v-btn @click="dialog = false" color="secondary">Cancelar</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-btn @click="dialog = true" color="primary">Agregar Elemento</v-btn>
    </div>
  </template>
  
  <script>
  import { defineComponent } from 'vue';
  import jsonData from '~/JsonData/bookDirectory.json'; // Importar el archivo JSON
  
  export default defineComponent({
    data() {
      return {
        treeItems: jsonData, // Utilizar los datos del archivo JSON
        openNodes: [], // Array para controlar los nodos abiertos en el árbol
        dialog: false, // Estado del diálogo para agregar elemento
        newItemName: '', // Nombre del nuevo elemento
        newItemType: 'directory', // Tipo del nuevo elemento (por defecto, directorio)
      };
    },
    methods: {
      toggleNode(node) {
        if (this.openNodes.includes(node.id)) {
          this.openNodes = this.openNodes.filter(id => id !== node.id);
        } else {
          this.openNodes.push(node.id);
        }
      },
      addItem() {
        // Agregar lógica para agregar el nuevo elemento al árbol
        const newItem = {
          id: Math.random().toString(36).substr(2, 9), // Generar un ID único
          name: this.newItemName,
          type: this.newItemType,
          children: [], // Array vacío para almacenar los archivos dentro del directorio
        };
        this.treeItems.push(newItem); // Agregar el nuevo elemento al árbol
        this.dialog = false; // Cerrar el diálogo
        this.newItemName = ''; // Reiniciar el nombre del elemento
      },
      deleteItem(item) {
        // Eliminar el elemento seleccionado del árbol
        const index = this.treeItems.indexOf(item);
        if (index !== -1) {
          this.treeItems.splice(index, 1);
        }
      },
    },
  });
  </script>
  