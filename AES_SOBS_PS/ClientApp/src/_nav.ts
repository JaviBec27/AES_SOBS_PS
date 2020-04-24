export const navigation = [
  {
    name: "Cockpit",
    url: "/dashboard",
    icon: "tir ti-cockpit ti-2x dark-icon"
  },
  {
    title: true,
    name: "__________________________________"
  },
  {
    name: "lblCalendar",
    url: "/#",
    icon: "tir ti-calendar ti-2x light-icon"
  },
  {
    name: "lblMeasurement",
    url: "/event",
    icon: "tir ti-measurement ti-2x dark-icon",
    children: [
      {
        name: "lblMeterView",
        url: "/meter-view",
        icon: "tir ti-flow-computer no_color"
      }
    ]
  },
  {
    name: "lblIndicators",
    url: "/indicators-view",
    icon: "tir ti-project-tracker  dark-icon"
  },
  {
    name: "lblMaintenance",
    url: "/#",
    icon: "tir ti-test ti-2x light-icon"
  },
  {
    name: "lblReports",
    url: "/#",
    icon: "tir ti-crm-forms ti-2x light-icon"
  }
];
