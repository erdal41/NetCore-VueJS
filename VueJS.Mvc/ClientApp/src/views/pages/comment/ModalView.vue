<template>
    <div>
        <b-modal id="modal-view"
                 ok-only
                 ok-variant="outline-secondary"
                 ok-title="Kapat"
                 centered>
            <b-overlay :show="comment === null"
                       rounded="sm">
                <template #modal-header>
                    <div class="content-header-left"
                         cols="6">
                        <h3 class="modal-title float-left">
                            Yorum Önizleme
                        </h3>
                    </div>
                    <div class="content-header-right text-md-right d-md-block d-none"
                         cols="6">
                        <b-badge v-if="comment.CommentStatus == 1"
                                 variant="success">
                            Onaylandı
                        </b-badge>
                        <b-badge v-if="comment.CommentStatus == 0"
                                 variant="warning">
                            İnceleme Bekliyor
                        </b-badge>
                    </div>
                </template>

                <b-row>
                    <b-col cols="6">
                        <b-form-group label="Yorum Yapan">
                            <b-form-input disabled
                                          :value="comment.Name"></b-form-input>
                        </b-form-group>
                    </b-col>
                    <b-col cols="6">
                        <b-form-group label="Email">
                            <b-form-input disabled
                                          :value="comment.Email"></b-form-input>
                        </b-form-group>
                    </b-col>
                    <b-col cols="12">
                        <b-form-group label="Yorum İçeriği">
                            <b-form-textarea disabled
                                             :value="comment.Text"></b-form-textarea>
                        </b-form-group>
                    </b-col>
                </b-row>
            </b-overlay>
        </b-modal>
    </div>

</template>

<script>
    import { BOverlay, BRow, BCol, BBadge, BFormGroup, BFormInput, BFormTextarea } from 'bootstrap-vue'

    export default {
        components: {
            BOverlay,
            BRow,
            BCol,
            BBadge,
            BFormGroup,
            BFormInput,
            BFormTextarea
        },
        props: {
            comment: {
                type: [],
            },
        }
    }
</script>