import { CSSProperties } from 'react'

const defaultMarkerStyle: CSSProperties = {
	left: 0,
	top: 0,
	position: 'absolute',
}

export const mapStyles: { [key: string]: CSSProperties } = {
	clusterMarker: {
		alignItems: 'center',
		backgroundColor: '#51D5A0',
		border: '2px solid #56C498',
		borderRadius: '50%',
		color: 'white',
		cursor: 'pointer',
		display: 'flex',
		justifyContent: 'center',
		height: 30,
		width: 30,
		...defaultMarkerStyle,
	},
	marker: {
		alignItems: 'center',
		backgroundImage: 'url("http://www.iconninja.com/files/139/538/395/pigeon-bird-shape-icon.png")',
		backgroundRepeat: 'no-repeat',
		backgroundSize: 'contain',
		border: '2px solid rgba(255, 255, 255, 0)',
		borderRadius: '50%',
		display: 'flex',
		justifyContent: 'center',
		height: 30,
		width: 30,
		...defaultMarkerStyle,
	},
}

export const popupStyles: CSSProperties = {
	position: 'fixed',
	left: 0,
	top: 0,
}
