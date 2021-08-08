import { CSSProperties } from 'react'

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
	},
}

export const absoluteMapStyles: { [key: string]: CSSProperties } = {
	clusterMarker: {
		left: 10,
		top: 60,
		position: 'absolute',
	},
	marker: {
		left: 10,
		top: 60,
		position: 'absolute',
	},
}

export const popupStyles: CSSProperties = {
	position: 'fixed',
	left: 0,
	top: 0,
}
