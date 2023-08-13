let wakeLockType = -1;
if ('WakeLock' in window && 'request' in window.WakeLock) {  
  wakeLockType = 0; 
} else if ('wakeLock' in navigator && 'request' in navigator.wakeLock) { 
  wakeLockType = 1; 
}else
{
  console.error('Wake Lock API not supported.');
}

let wakeLock = null;
const requestWakeLockWindow = () => {
  const controller = new AbortController();
  const signal = controller.signal;
  window.WakeLock.request('screen', {signal})
  .catch((e) => {      
    if (e.name === 'AbortError') {        
      console.log('Window Wake Lock was aborted');
    } else {
      console.error(`${e.name}, ${e.message}`);
    }
  });
  console.log('Window Wake Lock is active');
  return controller;
};

const requestWakeLockNavigator = async() => {
  try {
    wakeLock = await navigator.wakeLock.request('screen');
    wakeLock.addEventListener('release', (e) => {
      console.log(e);
      console.log('Navigator Wake Lock was released');                    
    });
    console.log('Navigator Wake Lock is active');      
  } catch (e) {      
    console.error(`${e.name}, ${e.message}`);
  } 
};

const requestWakeLock = () => {
  switch(wakeLockType)
  {
    case 0:
      return requestWakeLockWindow();
      break;
    case 1:
      requestWakeLockNavigator();
      break;
    default:
      break;  
  }
  
};  

const turnOnWakeLock = () => {
  switch(wakeLockType)
  {
    case 0:
      wakeLock = requestWakeLock();
      break;
    case 1:
      requestWakeLock();
      break;
    default:
      break;
  }
  
};

const turnOffWakeLock = () => {
  switch(wakeLockType)
  {
    case 0:
      if(wakeLock != null)
      {
        wakeLock.abort();
        wakeLock = null;
      }
      break;
    case 1:
      if(wakeLock != null)
      {
        wakeLock.release();
        wakeLock = null;
      }
      break;
  }
  
};

const handleVisibilityChange = () => {    
  if (wakeLock !== null && document.visibilityState === 'visible') {
      turnOnWakeLock();
  }
};    

document.addEventListener('visibilitychange', handleVisibilityChange);