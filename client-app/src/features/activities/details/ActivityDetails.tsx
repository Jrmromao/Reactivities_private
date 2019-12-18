import React, { useContext, useEffect } from "react";
import {  Grid } from "semantic-ui-react";
import ActivityStore from "../../../app/stores/activityStore";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import  ActivityDetailsHeader from "./ActivityDetailsHeader";
import { ActivityDetailedInfo } from "./ActivityDetailedInfo";
import { ActivityDetailedChat } from "./ActivityDetailedChat";
import { ActivityDetailedSidebar } from "./ActivityDetailedSidebar";
 

interface IDetailParams{
  id: string
}


const ActivityDetails: React.FC<RouteComponentProps<IDetailParams>> = ({
  match,
  history
}) => {
  const activityStore = useContext(ActivityStore);
  const { activity, loadactivity, loadingInitial } = activityStore;

  useEffect(() => {
    loadactivity(match.params.id);
  }, [loadactivity, match.params.id, history]); // to run only one time

if(loadingInitial) 
return <LoadingComponent content='Loading activity...'/>

if(!activity)
  return <h2>Activity not found</h2>

  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityDetailsHeader activity={activity}/>
        <ActivityDetailedInfo activity={activity}/>
        <ActivityDetailedChat />
      </Grid.Column>
      <Grid.Column width={6}>
        <ActivityDetailedSidebar />
      </Grid.Column>
    </Grid>
  );
};
export default observer(ActivityDetails);
