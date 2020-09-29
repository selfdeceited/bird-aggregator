import * as React from 'react'

const styles: { [key: string]: React.CSSProperties } = {
	barWrap: {
		marginTop: '5px',
	},
	clusterMarker: {
		alignItems: 'center',
		backgroundColor: '#51D5A0',
		border: '2px solid #56C498',
		borderRadius: '50%',
		color: 'white',
		cursor: 'pointer',
		display: 'flex',
		height: 30,
		justifyContent: 'center',
		width: 30,
	},
	marker: {
		alignItems: 'center',
		backgroundImage: 'url("http://www.iconninja.com/files/139/538/395/pigeon-bird-shape-icon.png")',
		backgroundRepeat: 'no-repeat',
		backgroundSize: 'contain',
		border: '2px solid rgba(255, 255, 255, 0)',
		borderRadius: '50%',
		display: 'flex',
		height: 30,
		justifyContent: 'center',
		width: 30,
	},
	seedPopover: {
		padding: '10px',
	},
}

export default styles
