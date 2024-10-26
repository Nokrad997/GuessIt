import { getUsage, updateUsage } from "../api/monthlyUsageApi";
import { useError } from "../components/ErrorContext/ErrorContext";

const useMonthlyUsage = () => {
    const { triggerError } = useError();
    const updateMonthlyUsage = async (userUsage: number) => {
        try{
            console.log(userUsage)
            const response = await updateUsage(userUsage);
            return response;
        } catch(error: any){
            triggerError('Failed to update monthly usage.');
            console.log(error);
        }
    };

    const getMonthlyUsage = async () =>{
        try{
            const response = await getUsage();
            return response.monthlyUsage;
        } catch(error: any){
            triggerError('Failed to get monthly usage.');
            console.log(error);
        }
    }

    return {
        updateMonthlyUsage,
        getMonthlyUsage
    };
};

export default useMonthlyUsage;
