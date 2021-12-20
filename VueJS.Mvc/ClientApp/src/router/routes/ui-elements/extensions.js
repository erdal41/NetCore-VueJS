export default [
  {
    path: '/extensions/sweet-alert',
    name: 'extensions-sweet-alert',
    component: () => import('@/views/admin/extensions/sweet-alert/SweetAlert.vue'),
    meta: {
      pageTitle: 'Sweet Alerts',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'Sweet Alerts',
          active: true,
        },
      ],
    },
  },

  {
    path: '/extensions/drag-drop',
    name: 'extensions-drag-and-drop',
    component: () => import('@/views/admin/extensions/drag-drop/DragDrop.vue'),
    meta: {
      pageTitle: 'Drag & Drop',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'Drag & Drop',
          active: true,
        },
      ],
    },
  },
  {
    path: '/extensions/swiper',
    name: 'extensions-swiper',
    component: () => import('@/views/admin/extensions/swiper/Swiper.vue'),
    meta: {
      pageTitle: 'Swiper',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'Swiper',
          active: true,
        },
      ],
    },
  },
  {
    path: '/extensions/clipboard',
    name: 'extensions-clipboard',
    component: () => import('@/views/admin/extensions/clipboard/Clipboard.vue'),
    meta: {
      pageTitle: 'Clipboard',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'Clipboard',
          active: true,
        },
      ],
    },
  },
  {
    path: '/extensions/context-menu',
    name: 'extensions-context-menu',
    component: () => import('@/views/admin/extensions/context-menu/ContextMenu.vue'),
    meta: {
      pageTitle: 'Context Menu',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'Context Menu',
          active: true,
        },
      ],
    },
  },
  {
    path: '/extensions/toastification',
    name: 'extensions-toastification',
    component: () => import('@/views/admin/extensions/toastification/Toastification.vue'),
    meta: {
      pageTitle: 'Toastification',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'Toastification',
          active: true,
        },
      ],
    },
  },
  {
    path: '/extensions/i18n',
    name: 'extensions-i18n',
    component: () => import('@/views/admin/extensions/i18n/i18n.vue'),
    meta: {
      pageTitle: 'I18n',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'I18n',
          active: true,
        },
      ],
    },
  },
  {
    path: '/extensions/slider',
    name: 'extensions-slider',
    component: () => import('@/views/admin/extensions/slider/Slider.vue'),
    meta: {
      pageTitle: 'Slider',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'Slider',
          active: true,
        },
      ],
    },
  },
  {
    path: '/extensions/tour',
    name: 'extensions-tour',
    component: () => import('@/views/admin/extensions/tour/Tour.vue'),
    meta: {
      pageTitle: 'Tour',
      breadcrumb: [
        {
          text: 'Extensions',
        },
        {
          text: 'Tour',
          active: true,
        },
      ],
    },
  },
  // {
  //   path: '/extensions/tree',
  //   name: 'extensions-tree',
  //   component: () => import('@/views/admin/extensions/tree/Tree.vue'),
  //   meta: {
  //     pageTitle: 'Tree',
  //     breadcrumb: [
  //       {
  //         text: 'Extensions',
  //       },
  //       {
  //         text: 'Tree',
  //         active: true,
  //       },
  //     ],
  //   },
  // },
]
