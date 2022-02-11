export default [
    {
        title: 'Dashboards',
        icon: 'HomeIcon',
        tag: '2',
        tagVariant: 'light-warning',
        children: [
            {
                title: 'eCommerce',
                route: 'dashboard-ecommerce',
                resource: 'Dashboard',
                action: 'read',
            },
            {
                title: 'Analytics',
                route: 'dashboard-analytics',
                resource: 'Dashboard',
                action: 'read',
            },
        ],
    },
]
