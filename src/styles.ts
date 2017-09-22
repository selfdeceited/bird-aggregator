import * as React from "react"

const styles: { [key: string]: React.CSSProperties } = {
    clusterMarker: {
      width: 30,
      height: 30,
      borderRadius: '50%',
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      color: 'white',
      border: '2px solid #56C498',
      cursor: 'pointer',
      backgroundColor: '#51D5A0',
    },
    marker: {
      width: 30,
      height: 30,
      borderRadius: '50%',
      backgroundImage: 'url("http://www.iconninja.com/files/139/538/395/pigeon-bird-shape-icon.png")',
      backgroundRepeat: 'no-repeat',
      backgroundSize: 'contain',
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      border: '2px solid rgba(255, 255, 255, 0)'
    }
}
export default styles;