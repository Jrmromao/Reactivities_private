export const combineDateTime = (date: Date, time: Date) => {


    // build up a date/time string and combine them together 
    const timeString = time.getHours() + ':' + time.getMinutes() + ':00';
    const year = date.getFullYear();
    const month = date.getMonth();
    const day = date.getDate() + 1;
    const dateString = `${year}-${month}-${day}`;


    return new Date(dateString + ' ' + timeString);


}