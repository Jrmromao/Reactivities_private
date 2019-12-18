import { toast } from 'react-toastify';
import { SyntheticEvent } from 'react';
import { IActivity } from './../models/activity';
import { observable, action, computed, configure, runInAction } from 'mobx';
import { createContext } from 'react';
import agent from '../API/agent';
import { history } from '../..';


configure({ enforceActions: 'always' })

class ActivityStore {
    // w3 
    @observable loadingInitial = false;
    @observable activity: IActivity | null = null;
    @observable submitting = false;
    @observable target = '';
    // this observable 
    @observable activityRegistry = new Map();
    // computed prop to sort the activities by date
    @computed get activitiesByDate() {
        // return the array sorted by date in ascending order
        return  this.groupActivitiesVyDate(Array.from(this.activityRegistry.values()));
    }

    groupActivitiesVyDate(activities: IActivity[]) {
        const sortedActivities = activities.sort((a, b) => a.date.getTime() - b.date.getTime());
        return Object.entries(sortedActivities.reduce((activities, activity) =>{
            const date = activity.date.toISOString().split('T')[0];
            activities[date] = activities[date] ? [...activities[date], activity] : [activity];

            return activities;
        }, {} as {[key: string]: IActivity[]}));

    }
    @action selectActivity = (id: string) => {
        this.activity = this.activityRegistry.get(id);
    }
    @action loadActivities = async () => {
        this.loadingInitial = true;
        // now lets use our agent to get the activities
        // promess based API. bnow we know that our request is getting an IActivity type
        try {
            const activities = await agent.Activities.list();
            runInAction('loading activities', () => {
                    activities.forEach(activity => {
                    activity.date = new Date(activity.date);
                    this.activityRegistry.set(activity.id, activity);
                });
                this.loadingInitial = false;
            });
        
        } catch (error) {
            runInAction('load activities error', () => {
                this.loadingInitial = false;
            });
           
        }
    };
    @action createActivity = async (activity: IActivity) => {
        this.submitting = true;
        try {
            await agent.Activities.create(activity);
            runInAction('creating activity', () => {
                this.activityRegistry.set(activity.id, activity);
                this.submitting = false;
            });
            history.push(`/activities/${activity.id}`)
        } catch (error) {
            runInAction('reate activity error', () => {
                this.submitting = false;
            });
            toast.error('Problem submitting data');
            console.log(error.response)
        }
    };
    @action editActivity = async (activity: IActivity) => {
        this.submitting = true;
        try {
            await agent.Activities.update(activity);
            runInAction('editing activity', () => {
                this.activityRegistry.set(activity.id, activity);
                this.activity = activity;
                this.submitting = false;
            });
            history.push(`/activities/${activity.id}`)
        } catch (error) {
            runInAction('edit acticity error', () => {
                this.submitting = false;
            });
            toast.error('Problem submitting data');
            console.log(error.response)
        }
    }
    @action deleteActivity = async (event: SyntheticEvent<HTMLButtonElement>, id: string) => {
        this.submitting = true;
        this.target = event.currentTarget.name;
        try {
            await agent.Activities.delete(id);
            runInAction('deleting Activity', () => {
                this.activityRegistry.delete(id);
                this.submitting = false;
                this.target = '';
            });
        } catch (error) {
            runInAction('delete acticity error', () => {
                this.submitting = false;
                this.target = '';
            });
            console.log(error)
        }
    }
    @action loadactivity = async (id: string) => {
        let activity = this.getActivity(id);

        if (activity)
        {    this.activity = activity;
            return activity;
        }
        else {
            this.loadingInitial = true;
            try {
                activity = await agent.Activities.details(id);
                runInAction('getting activity', () => {
                    activity.date = new Date(activity.date);
                    this.activity = activity;
                    this.activityRegistry.set(activity.id, activity);
                    this.loadingInitial = false;
                });
                return activity;
            } catch (error) {
                runInAction('getting activity error', () => {
                    this.loadingInitial = false;
                });
              
              console.log(error);
                
            }
        }
    }
    @action clearActivity = () => {
        this.activity = null;
    }
    getActivity = (id: string) => {
        return this.activityRegistry.get(id);
    }


}

export default createContext(new ActivityStore())