import { Status } from "./Status";

export interface TaskResult {
    products: string[];
    status: Status;
    totalCalories: number;
}
