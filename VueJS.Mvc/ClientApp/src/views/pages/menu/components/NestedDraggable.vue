<template>
    <draggable v-bind="dragOptions"
               class="item-container list-group list-group-flush"
               :list="menuDetails"
               tag="ul"
               :move="checkMove">
            <b-list-group-item v-for="el in menuDetails"
                               :key="el.Id"
                               class="item-group list-unstyled p-0 border-0"
                               tag="li">
                <div class="item cursor-move">
                    <span class="cursor-pointer">
                        {{ el.CustomName }}
                    </span>
                    <feather-icon v-if="isOpenCollapse == false"
                                  v-b-toggle="'item_' + el.Id"
                                  icon="ChevronDownIcon"
                                  size="16"
                                  class="float-right"
                                  @click="isOpenCollapse = !isOpenCollapse" />
                    <feather-icon v-if="isOpenCollapse == true"
                                  v-b-toggle="'item_' + el.Id"
                                  icon="ChevronUpIcon"
                                  size="16"
                                  class="float-right"
                                  @click="isOpenCollapse = !isOpenCollapse" />
                    <small class="text-muted float-right"> {{ menuType(el.ObjectType) }}</small>
                    <small class="float-right mr-1 cursor-pointer text-danger"
                           @click.prevent="menuItemDelete(el)">Sil</small>
                </div>
                <b-collapse :id="'item_' + el.Id">
                    <div class="menu-item-collapse">
                        <b-form-group label="Menü Etiketi"
                                      :label-for="'menu-tag-' + el.Id">
                            <b-form-input :id="'menu-tag-' + el.Id"
                                          v-model="el.CustomName"
                                          type="text"
                                          size="sm"
                                          placeholder="Menü Etiketi"
                                          @blur="updateMenuItems" />
                        </b-form-group>
                        <label>URL: </label> <a class="small" :href="el.CustomURL" target="_blank">{{ el.CustomURL }}</a>
                    </div>
                </b-collapse>
                <nested-draggable class="item-sub item-container list-group list-group-flush"
                                  :menuDetails="el.Children"
                                  v-bind="dragOptions"/>
            </b-list-group-item>
    </draggable>
</template>

<script>
    import draggable from "vuedraggable";
    import { BListGroupItem, BCollapse, VBToggle , BFormGroup, BFormInput } from 'bootstrap-vue';
    import axios from 'axios'
    export default {
        name: "nested-draggable",
        components: {
            draggable,
            BListGroupItem,
            BCollapse,
            BFormGroup,
            BFormInput
        },
        directives: {
            'b-toggle': VBToggle
        },
        props: {
            menuDetails: {
                type: Array
            },
        },
        data() {
            return {
                menuItemsText: [],
                isOpenCollapse: false,
                drag: false
            }
        },
        methods: {
            getData() {
                console.log(this.menuDetails);
            },
            updateMenuItems() {
                this.$emit('updateMenuItem', this.menuDetails);
            },
            menuItemDelete(menuDetailItem) {
                axios.post('/admin/menu-deletemenudetail?menuId=' + menuDetailItem.Id).then((response) => {
                    let index = this.menuDetails.indexOf(menuDetailItem);
                    let children = [];
                    if (response.data.ResultStatus === 0) {
                        this.menuDetails.forEach(el => {
                            if (menuDetailItem.Id == el.Id) {
                                if (menuDetailItem.Children.length > 0) {
                                    for (var i = 0; i < menuDetailItem.Children.length; i++) {
                                        children.push(menuDetailItem.Children[i]);
                                    }
                                }
                            }
                        });
                        this.menuDetails.splice(index, 1);
                        for (var i = 0; i < children.length; i++) {
                            this.menuDetails.splice(index + i, 0, children[i]);
                        }

                        console.log(index);
                        console.log(menuDetailItem);
                        console.log(this.menuDetails);
                    }
                }).catch((error) => {
                    console.log(error)
                    console.log(error.request)
                });
            },
            childList(id) {
                return this.menuDetails.filter(item => item.ParentId === id);
            },
            menuType(type) {
                if (type === 0) {
                    return 'Sayfa';
                } else if (type === 1) {
                    return 'Makale';
                } else if (type === 2) {
                    return 'Kategori';
                } else if (type === 3) {
                    return 'Etiket';
                } else if (type === 4) {
                    return 'Temel Sayfa';
                } else if (type === null) {
                    return 'Özel Bağlantı';
                }
            },
            checkMove: function (event) {
                console.log(event)
                console.log(this)
            }
        },
        computed: {
            dragOptions() {
                return {
                    animation: 200,
                    group: "description",
                    disabled: false,
                    ghostClass: "ghost"
                };
            }
        },
        mounted() {
            this.getData();
        }
    };
</script>

<style scoped>
    .item-container {
        max-width: 30rem;
        margin: 0;
    }

    .item {
        padding: 10px 10px 10px 15px;
        box-shadow: 0px 0px 3px 0px #5e5e5e;
        background-color: #f6f7f7;
    }

    .item-sub {
        margin: 0 0 0 1rem;
    }

    .menu-item-collapse {
        padding: 10px;
        border: 1px solid #b7b7b7;
    }

    .flip-list-move {
        transition: transform 0.5s;
        border-top-color:red;
    }

    .no-move {
        transition: transform 0s;
    }

    .ghost {
        opacity: 0.5;
        background: red;
    }
</style>