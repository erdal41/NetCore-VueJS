<template>
    <draggable v-bind="dragOptions"
               tag="div"
               class="item-container">
        <div class="item-group" :key="el.Id" v-for="el in list" :value="list">
            <div class="item cursor-move">
                <b-form-checkbox v-if="isItemCheck"
                                 v-model="checkMenuItems"
                                 class="d-inline"
                                 :value="{Id: el.Id, Name:el.CustomName + '(' + menuType(el.SubObjectType) + ')' }"
                                 @change="checkChangeMenuItem" />
                <span class="cursor-pointer">{{ el.CustomName }}</span>
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
                <small class="text-muted float-right"> {{ menuType(el.SubObjectType) }}</small>
                <small class="float-right mr-1 cursor-pointer text-danger"
                       @click.prevent="menuItemDelete(el.Id)">Sil</small>
            </div>
            <b-collapse :id="'item_' + el.Id">
                <div class="menu-item-collapse">
                    <b-form-group label="Menü Etiketi"
                                  :label-for="'menu-tag-' + el.Id">
                        <b-form-input :id="'menu-tag-' + el.Id"
                                      :value="el.CustomName"
                                      type="text"
                                      size="sm"
                                      placeholder="Menü Etiketi" />
                    </b-form-group>
                    <label>URL: </label> <a class="small" :href="el.CustomURL" target="_blank">{{ el.CustomURL }}</a>
                </div>
            </b-collapse>
            <nested-draggable class="item-sub" :list="el.Children" />
        </div>
    </draggable>
</template>

<script>
    import draggable from "vuedraggable";
    import AppCollapse from '@core/components/app-collapse/AppCollapse.vue'
    import AppCollapseItem from '@core/components/app-collapse/AppCollapseItem.vue'
    import { BCollapse, VBToggle, BFormCheckbox, BFormGroup, BFormInput } from 'bootstrap-vue';
    import axios from 'axios'
    export default {
        name: "nested-draggable",
        components: {
            draggable,
            AppCollapse,
            AppCollapseItem,
            BCollapse,
            BFormCheckbox,
            BFormGroup,
            BFormInput
        },
        directives: {
            'b-toggle': VBToggle
        },
        props: {
            list: {
                required: false,
                type: Array,
                default: null
            },
            isMenuItemCheck: {
                type: Boolean
            }
        },
        data() {
            return {
                isOpenCollapse: false,
                checkMenuItems: []
            }
        },
        methods: {
            checkChangeMenuItem() {
                this.$emit('deleteMenuItem', this.checkMenuItems);
            },
            menuItemDelete(id) {
                axios.post('/admin/menu-deletemenudetail?menuId=' + id).then((response) => {
                    if (response.data.ResultStatus === 0) {
                        this.$emit('menuItemDeleted', id);
                    }
                });
            },
            menuType(type) {
                if (type === 0) {
                    return 'Sayfa';
                } else if (type === 1) {
                    return 'Makale';
                }
                else if (type === 2) {
                    return 'Kategori';
                } else if (type === 4) {
                    return 'Sayfa';
                } else if (type === null) {
                    return 'Özel Bağlantı';
                }
            }         
        },
        computed: {
            isItemCheck() {
                if (this.isMenuItemCheck === true) {
                    return true;
                } else {
                    this.checkMenuItems = [];
                    return false;
                }
            },
            dragOptions() {
                return {
                    animation: 0,
                    group: "description",
                    disabled: false,
                    ghostClass: "ghost"
                };
            }
        },
        mounted() {
            console.log(this.list);
        }
    };
</script>

<style scoped>
    .item-container {
        max-width: 30rem;
        margin: 0;
    }

    .item-group {
        margin-top: 8px;
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
</style>