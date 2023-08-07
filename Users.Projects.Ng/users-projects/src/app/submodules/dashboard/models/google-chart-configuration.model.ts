import { ChartType, Column } from "angular-google-charts";

export interface GoogleChartConfiguration {
    type: ChartType.BarChart,
    columns: Column[]
}