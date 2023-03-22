function sideMenuItems(control_route, action_route) {
    return [
        {
            icon: 'bx bx-line-chart',
            link: '/HR/Statistic/Index',
            name: '儀錶板',
            isActive: control_route == 'Statistic' && action_route == 'Index',
            subItems: [],
        },
        {
            icon: 'bx bx-category',
            link: 'javascript:void(0)',
            name: '職務資訊',
            isActive: control_route == 'Job',
            subItems: [
                {
                    link: '/HR/Job/JobType',
                    name: '職務類別',
                    isActive: control_route == 'Job' && action_route == 'JobType',
                },
                {
                    link: '/HR/Job/Info',
                    name: '職缺資訊',
                    isActive: control_route == 'Job' && action_route == 'Info',
                }
            ],
        },
        {
            icon: 'bx bx-user-pin',
            link: '/HR/HumanResource/Index',
            name: '人才資料',
            isActive: control_route == 'HumanResource' && action_route == 'Index',
            subItems: [],
        },
        {
            icon: 'bx bx-book-content',
            link: '/HR/FrontendResume/Index',
            name: '履歷進件',
            isActive: control_route == 'FrontendResume' && action_route == 'Index',
            subItems: [],
        },
        {
            icon: 'bx bx-cog',
            link: 'javascript:void(0)',
            name: '參數建檔',
            isActive: control_route == 'Parameters',
            subItems: [
                {
                    link: '/HR/Parameters/SystemOption',
                    name: '系統選項維護',
                    isActive: control_route == 'Parameters' && action_route == 'SystemOption',
                },
                {
                    link: '/HR/Parameters/MessageMemo',
                    name: '訊息備忘錄',
                    isActive: control_route == 'Parameters' && action_route == 'MessageMemo',
                }
            ],
        },
        //{
        //    icon: 'bx bx-notepad',
        //    link: 'javascript:void(0)',
        //    name: '試用期考核',
        //    isActive: control_route == 'Probation',
        //    subItems: [
        //        {
        //            link: '/HR/Probation/CreateForm',
        //            name: '建立試用期考核單',
        //            isActive: control_route == 'Probation' && action_route == 'CreateForm',
        //        },
        //    ],
        //},
    ];
}