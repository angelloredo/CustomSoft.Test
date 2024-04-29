<template>
  <div>
    <h2>Directorio de libros</h2>
    <br>
    <v-treeview :items="treeItems" activatable :open="openNodes" @update:active="onActiveItemChange($event)">
      <template v-slot:prepend="{ item }">
        <v-icon>{{ item.type === 'directory' ? 'mdi-folder' : 'mdi-file' }}</v-icon>
        <span @click="toggleNode(item)">{{ item.name }}</span>
      </template>
      <template v-slot:label="{ item }">

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
    <v-btn @click="dialog = true" color="primary" v-if="activeItem == null || activeItem.type === 'directory'">Agregar
      Elemento</v-btn>
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
      newItemType: 'directory', // Tipo del nuevo elemento (por defecto, directorio),
      activeItemId: [] // Elemento activo
    };
  },
  computed: {
    activeItem() {
      if (this.activeItemId.length == 0) {
        return null;
      }
      return this.findElementById();

    }
  },
  methods: {
    findElementById() {
      let id = this.activeItemId[0];
      console.log("id" + id);
      let res = null;
      for (let item of this.treeItems) {

        if (item.id == id) {
          return item;
          break;
        }

        if (item.children && item.children.length > 0) {
          for (let itemChild of item.children) {

            if (itemChild.id == id) {
              res = itemChild;
              break;
            }
          }
        }
      }

      return res;
    },
    onActiveItemChange(item) {
      console.log(item);
      this.activeItemId = item;
    },
    toggleNode(node) {
      if (node.type === 'directory') {
        if (this.openNodes.includes(node.id)) {
          this.openNodes = this.openNodes.filter(id => id !== node.id);
        } else {
          this.openNodes.push(node.id);
        }
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

      if (this.activeItem != null) {
        this.activeItem.children.push(newItem); // Agregar el nuevo elemento al árbol

      } else {
        this.treeItems.push(newItem); // Agregar el nuevo elemento al árbol

      }
      this.dialog = false; // Cerrar el diálogo
      this.newItemName = ''; // Reiniciar el nombre del elemento
    },
    deleteItem(item) {
      // Eliminar el elemento seleccionado del árbol
      const itemId = item.id;

      const deleteItemAndChildren = (items) => {
        for (let i = 0; i < items.length; i++) {
          if (items[i].id === itemId) {
            items.splice(i, 1);
            return true; // Elemento encontrado y eliminado
          }
          if (items[i].children && items[i].children.length > 0) {
            if (deleteItemAndChildren(items[i].children)) {
              return true; // Elemento encontrado y eliminado en los hijos
            }
          }
        }
        return false; // Elemento no encontrado en este nivel
      };

      deleteItemAndChildren(this.treeItems);

    },
  },
});
</script>